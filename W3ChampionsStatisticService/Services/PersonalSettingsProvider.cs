using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MongoDB.Driver;
using W3ChampionsStatisticService.Cache;
using W3ChampionsStatisticService.ReadModelBase;

namespace W3ChampionsStatisticService.Services
{
    public class PersonalSettingsProvider : MongoDbRepositoryBase
    {
        public static CachedData<List<PersonalSettings.PersonalSetting>> personalSettingsCache;

        public PersonalSettingsProvider(MongoClient mongoClient) : base(mongoClient)
        {
            personalSettingsCache = new CachedData<List<PersonalSettings.PersonalSetting>>(() => FetchPersonalSettingsSync(), TimeSpan.FromMinutes(10));
        }
        
        public List<PersonalSettings.PersonalSetting> getPersonalSettings()
        {
            try 
            {
                return personalSettingsCache.GetCachedData();
            }
            catch
            {
                return new List<PersonalSettings.PersonalSetting>();
            }
        }

        public PersonalSettings.PersonalSetting getSettingsOf(string battleTag)
        {
            try 
            {
                var settings = personalSettingsCache.GetCachedData().Find(s => s.Id == battleTag);
                return settings;
            }
            catch
            {
                return new PersonalSettings.PersonalSetting(battleTag);
            }
        }

        private List<PersonalSettings.PersonalSetting> FetchPersonalSettingsSync()
        {
            try 
            {
                return FetchPersonalSettings().GetAwaiter().GetResult();
            }
            catch
            {
                return new List<PersonalSettings.PersonalSetting>();
            }
        }

        private Task<List<PersonalSettings.PersonalSetting>> FetchPersonalSettings()
        {
            return LoadAll<PersonalSettings.PersonalSetting>();
        }
    
    }
}