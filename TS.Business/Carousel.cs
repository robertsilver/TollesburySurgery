using System.Collections.Generic;
using TS.DataAccess;

namespace TS.Business
{
    public class Carousel
    {
        public static List<CarouselData> GetAll(string url)
        {
            return CarouselData.GetAll(url);
        }
    }
}
