using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GreeniumCommon;

namespace GreeniumCore.Addons {
    public class DllReader {
        private String Path { get; set; }

        public AssemblyName Name { get; private set; }
        public Assembly LoadedAssembly { get; private set; }

        public Type PType { get; private set; }
        private Boolean IsLoaded { get; set; } = false;
        private Type[] PluginTypes { get; set; }

        public DllReader(String path, bool load = false){
            Path = path;
            if (load)
                Load();
            
        }

        public bool Load(){
            if (IsLoaded) return IsLoaded;
            try {
                Name = AssemblyName.GetAssemblyName(Path);
                LoadedAssembly = Assembly.Load(Name);
                PType = typeof(IPlugin);
                IsLoaded = (LoadedAssembly != null);
            } catch {}

            return IsLoaded;
        }

        private Type[] GetTypes(){
            if (IsLoaded)
                return PluginTypes = LoadedAssembly.GetTypes();
            return null;
        }

        private IPlugin Generate(Type type){
            if (IsLoaded && type.GetInterface(PType.FullName) != null)
                return (IPlugin) Activator.CreateInstance(type);
            return null;
        }
    }
}
