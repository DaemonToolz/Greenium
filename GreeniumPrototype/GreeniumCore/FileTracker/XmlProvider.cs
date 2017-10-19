using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using GreeniumCore.FileTracker.FileTracker.Exception;


namespace GreeniumCore.FileTracker.FileTracker
{

    // Model possibly deprecated
    // https://stackoverflow.com/questions/31692333/multiple-users-writing-at-the-same-file
    // For real-time multiple I/O
    public class XmlProvider : IDisposable
    {
        // REFERENCE https://lennilobel.wordpress.com/2009/09/02/streaming-into-linq-to-xml-using-c-custom-iterators-and-xmlreader/
        // Designed namings: _ft{file}.xml
        #region Reader
        public String SourcePath { get; protected set; }
        protected String FileParser { get; set; }
        protected String TrackedFile { get; set; }
        protected String AbsoluteFile => SourcePath + String.Format(FileParser, TrackedFile) + ".xml";

        // NOT IMPLEMENTED YET
        public readonly static short XDocumentLimit = 10; // Higher than 10Mo, reading as a stream

            #endregion 

        #region InMemory File
        public XDocument OpenedDocument { get; protected set; }
        protected XDocument Rollback { get; set; }
        #endregion

        #region Tags
        protected HashSet<String> CachedTags { get; set; }
        protected Dictionary<int, String[]> KnownNodes { get; set; }
        #endregion

        public XmlProvider(String src)
        {
            SourcePath = src;
            CachedTags = new HashSet<String>();
        }

        public XmlProvider(String src, String parserType) : this(src)
        {
            FileParser = parserType;
        }

        public XmlProvider(String src, String parserType, String trackedFile) : this(src, parserType)
        {
            TrackedFile = trackedFile;
        }

        public XmlProvider(String src, String parserType, String trackedFile, Dictionary<int, String[]> tags) : this(src, parserType, trackedFile)
        {
            KnownNodes = tags;
        }

        public bool FileExists()
        {
            return File.Exists(AbsoluteFile);
        }

        public XDocument OpenFile(String trackedFile = null)
        {
            CloseFile(); // Force disposal here

            if (trackedFile != null) 
                TrackedFile = trackedFile;

            if (!File.Exists(AbsoluteFile))
            {
                OpenedDocument = new XDocument();
                AddNode(null);
                return OpenedDocument;
            }
            return Rollback = OpenedDocument = XDocument.Load(AbsoluteFile);
        }

        public virtual void CloseFile() {
            OpenedDocument = Rollback = null;
        }

        public void SaveFile()
        {

            OpenedDocument.Save(AbsoluteFile);
            Rollback = OpenedDocument;
        }

        public virtual bool DeleteNodeByTag(String key, String tag, String value)
        {
            try
            {
                OpenedDocument.Descendants(key)
                    .Where(n => ((string)n.Element(tag)).Equals(value))
                    .Remove();

                SaveFile();
                return true;
            }
            catch
            {
                OpenedDocument = Rollback;
                return false;
            }
        }

        public virtual bool AddNode(String key, params XmlElementProvider[] data)
        {
            try
            {
                List<XElement> xmlData = new List<XElement>();

                if(data.Any())
                    xmlData.AddRange(
                        data.Select(XmlElementProvider.Recursive_ConvertToXElement)
                    );

                if (OpenedDocument.Root == null)
                    OpenedDocument.Add(new XElement("root", OpenedDocument.Root));

                SaveFile();

                if (key == null)
                {
                    OpenedDocument.Root.Add(xmlData);
                }
                else
                {
                 
                    if (!OpenedDocument.Descendants(key).Any())
                        OpenedDocument.Root.Add(new XElement(key, xmlData));
                    else
                        OpenedDocument.Descendants(key).FirstOrDefault().Add(xmlData);
                }

                SaveFile();
                return true;
            }
            catch 
            {
                OpenedDocument = Rollback;
                return false;
            }
        }

        public virtual IEnumerable<XmlElementProvider> ReadNode(String key)
        {
            if (OpenedDocument?.Root == null) return new List<XmlElementProvider>();
            var comments = OpenedDocument.Descendants(key).Elements();
            

            return !comments.Any() ? 
                new List<XmlElementProvider>() :
                comments.Select(node => XmlElementProvider.Recursive_XmlElementProvider(node));
        }


        public virtual bool InsertNode(String Id, String key, params XmlElementProvider[] data)
        {
            try
            {
                (OpenedDocument.Descendants(key))
                    .SingleOrDefault(idAttr => ((String)idAttr.Attribute("id")).Equals(Id))?
                    .AddAfterSelf(data.Select(XmlElementProvider.Recursive_ConvertToXElement));

                SaveFile();
                return true;
            }
            catch
            {
                OpenedDocument = Rollback;
                return false;
            }
        }

        public virtual bool DeleteFile()
        {
            try
            {
                CloseFile();
                File.Delete(AbsoluteFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual IEnumerable<XContainer> GetNodes(String key){
            try {
                return OpenedDocument.Elements(key);
            }
            catch
            {
                return new List<XContainer>();
            }
        }

        public virtual IEnumerable<bool> UpdateMultipleNodes(String Id, String Key, Dictionary<String, String> data, bool LineChange = false)
        {
            var result = data.Select(pair => UpdateNode(Id, Key, pair.Key, pair.Value, LineChange));
            if (!LineChange)
                SaveFile();
            return result;
        }

        public virtual bool UpdateNode(String Id, String Key, String Name, String NewValue, bool SaveChanges = true)
        {
            try
            {

                var MyNode = (OpenedDocument.Descendants(Key)).SingleOrDefault(idAttr => ((String)idAttr.Attribute("id")).Equals(Id));
                MyNode.Element(Name).Value = NewValue;

                if (SaveChanges)
                    SaveFile();
                return true;
            }
            catch 
            {
                OpenedDocument = Rollback;
                return false;
            }
        }

        public virtual bool DeleteNode(String Id, String Key, String AttribName = "id")
        {
            try
            {
                
                (OpenedDocument.Descendants(Key))
                    .Where(idAttr => ((String)idAttr.Attribute(AttribName)).Equals(Id))
                    .Remove();
                 

                
                SaveFile();
                return true;
            }
            catch {
                OpenedDocument = Rollback;
                return false;
            }
        }

        public virtual void CheckTag(String tag)
        {
            if (!CachedTags.Contains(tag) && !KnownNodes.Values.Any(tagArr => tagArr.Contains(tag)))
                throw new UnsupportedTagException();

            CachedTags.Add(tag);

        }

            #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SaveFile();

                    CloseFile();

                    CachedTags.Clear();
                    KnownNodes.Clear();

                    CachedTags = null;
                    KnownNodes = null;
                    TrackedFile = null;
                    FileParser = null;
                }


                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

        }
        #endregion



        protected static bool DictionaryEqual<TKey, TValue>( 
            IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            return DictionaryEqual(first, second, null);
        }

        private static bool DictionaryEqual<TKey, TValue>(
            IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second,
            IEqualityComparer<TValue> valueComparer)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            foreach (var kvp in first)
            {
                TValue secondValue;

                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!secondValue.GetType().IsArray && !valueComparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }
    }
}