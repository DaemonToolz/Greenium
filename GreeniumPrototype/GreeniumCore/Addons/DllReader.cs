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
        public MethodInfo ExecutionMethod { get; private set; }
        public Type[] PluginTypes { get; private set; }

        private Boolean IsLoaded { get; set; } = false;
        
        public Object GeneratedObject { get; private set; }

        public DllReader(String path, bool load = false){
            Path = path;
            if (load)
                Load();
            
        }

        private void SecurityValidation() {
            
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

        public Type[] GetTypes(){
            if (IsLoaded)
                return PluginTypes = LoadedAssembly.GetTypes();
            return null;
        }

        private IPlugin Generate(Type type){
            if (IsLoaded && type.GetInterface(PType.FullName) != null)
                return (IPlugin) Activator.CreateInstance(type);
            return null;
        }

        private IPlugin Generate()
        {
            if (IsLoaded)
                return (IPlugin)Activator.CreateInstance(GetTypes().Single(ext => ext.Name.Contains("MyPluginDescriptor")));
            return null;
        }

        public void Init(){
            if (GeneratedObject == null) GeneratedObject = Generate();
        }

        public object Execute(Object[] Params)
        {
            if (GeneratedObject == null) GeneratedObject = Generate();
            return PType.GetMethod("Execute").Invoke(GeneratedObject, Params);
        }

        public ModuleModel LoadedModule()
        {
            return new ModuleModel()
            {
                Name = ModuleName,
                Image = ModuleImage,
                Details = ModuleDetails,
                Descriptor = ModuleDescriptor,
                TargetType = TargetType
            };
        }

        public String ModuleImage => (String)(PType.GetProperty("Image").GetValue(GeneratedObject));
        public String ModuleName => (String)(PType.GetProperty("Name").GetValue(GeneratedObject));
        public String ModuleDescriptor => (String)(PType.GetProperty("Descriptor").GetValue(GeneratedObject));
        public String ModuleDetails => (String)(PType.GetProperty("Details").GetValue(GeneratedObject));

        public TargetType TargetType => (TargetType)(PType.GetProperty("TargetType").GetValue(GeneratedObject));
    }
}
