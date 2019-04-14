using System.Collections.Generic;
using TollesburySurgery.DataAccess;

namespace TollesburySurgery.Business
{
    public class Carousel
    {
        public static List<CarouselData> GetAll(string url)
        {
            return CarouselData.GetAll(url);
        }
    }
}
