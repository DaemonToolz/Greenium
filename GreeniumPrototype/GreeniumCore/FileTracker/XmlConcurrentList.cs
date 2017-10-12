using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreeniumCore.FileTracker.FileTracker
{
    public class XmlConcurrentList<T>{
        private readonly  List<T> _list = new List<T>();
        private readonly object _sync = new object();

        public void Add(T value) {
            lock (_sync)
                _list.Add(value);
            
        }

        public T Find(Predicate<T> predicate)
        {
            lock (_sync)
                return _list.Find(predicate);
            
        }

        public int Remove(Predicate<T> predicate){
            lock (_sync)
                return _list.RemoveAll(predicate);
        }

        public T FirstOrDefault()
        {
            lock (_sync)
                return _list.FirstOrDefault();
        }

        public List<T> ToList()
        {
            List<T> Result = new List<T>();
            lock (_sync)
            {
                foreach (var element in _list)
                    Result.Add(element);
            }
            return Result;
        }
    }
}