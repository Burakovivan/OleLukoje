using OleLukoje.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;

namespace OleLukoje.Helpers
{
    public class TimerModule : IHttpModule
    {
        static Timer timer;
        long interval = 30000; //30 секунд
        static object synclock = new object();
        static bool check = false;

        public void Init(HttpApplication app)
        {
            timer = new Timer(new TimerCallback(CheckStateAds), null, 0, interval);
        }

        private void CheckStateAds(object obj)
        {
            lock (synclock)
            {
                if (DateTime.Now.Hour == 2 && check == false)
                {
                    using (OleLukojeContext db = new OleLukojeContext())
                    {
                        IEnumerable<Ad> ads = db.Ads
                            .Where(ad => ad.StateAd == State.Active)
                            .AsEnumerable()
                            .Select(ad =>
                            {
                                if (ad.DateAd.AddMinutes(5) <= DateTime.Now)
                                {
                                    ad.StateAd = State.TemporarilyBlocked;
                                }
                                return ad;
                            });
                        foreach (Ad ad in ads)
                        {
                            db.Entry(ad).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    check = true;
                }
                else if (DateTime.Now.Hour != 2)
                {
                    check = false;
                }
            }
        }

        public void Dispose()
        { }
    }
}