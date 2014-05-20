using System;
using System.Collections.Generic;
using System.Linq;
using Cdnvn.Phim.Entities;

namespace Cdnvn.Phim.Web.Models
{
    public class GenericModel
    {
        private readonly FilmContext _context = new FilmContext();

        public static List<Setting> SetGeneric<T>(T input, IQueryable<Setting> _context) where T : class , new()
        {
            var inp = input.GetType();
            var r = new List<Setting>();
            var settings = _context.Where(s => s.Group == inp.Name);
            
            foreach (var propertyInfo in inp.GetProperties())
            {
                bool flag = true;
                foreach (var setting in settings)
                {
                    string temp = propertyInfo.Name;
                    if (setting.Keyword.Equals(propertyInfo.Name))
                    {
                        setting.Value = (string)propertyInfo.GetValue(input, null);
                        r.Add(setting);
                        flag = false;
                    }
                }

                if(flag)
                {
                    var _setting = new Setting();
                    _setting.Group = inp.Name;
                    _setting.Keyword = propertyInfo.Name;
                    _setting.Value = (string)propertyInfo.GetValue(input, null);
                    flag = true;
                    r.Add(_setting);
                }
                


            }
            return r;
        }

        public static T GetGeneric<T>(IQueryable<Setting> _context) where T : class , new()
        {
            var output = new T();
            var table = output.GetType().Name;
            var _settings = _context.Where(s => s.Group.Equals(table)).ToList();
            foreach (var propertyInfo in output.GetType().GetProperties())
            {
                foreach (var setting in _settings)
                {
                    if (propertyInfo.Name.Equals(setting.Keyword))
                    {
                        var value = Convert.ChangeType(setting.Value, propertyInfo.PropertyType);
                        propertyInfo.SetValue(output, value, null);
                    }
                }
            }
            return output;
        }
    }
}