using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SimpleBlog2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBlog2
{
    public class Database
    {
        private static ISessionFactory _sessionFactory;

        public static String SessionKey = "simpleBlog";


        public static ISession Session { 
            get {
                return (ISession)HttpContext.Current.Items[SessionKey];
            } 
        }

        public static void Configure()
        {
            var config = new Configuration();

            var mapping = new ModelMapper();

            mapping.AddMapping<UserMap>();
            mapping.AddMapping<RoleMap>();
            mapping.AddMapping<TagMap>();
            mapping.AddMapping<PostMap>();
        


            config.AddMapping(mapping.CompileMappingForAllExplicitlyAddedEntities());

            _sessionFactory = config.BuildSessionFactory();

        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }


        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;

            if (session != null)
            {
                session.Close();
            }

            HttpContext.Current.Items.Remove(SessionKey);
        }

    }
}