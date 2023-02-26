namespace HomeTask1
{
    class DataComparer
    {
        public List<CityData> CompareCityDatas(List<CityData> cityDatas)
        {
            List<CityData> resCityDatas = new();

            var res = cityDatas.GroupBy(sData => sData.city);
            foreach (var group in res)
            {
                CityData toAdd = new CityData();
                toAdd.services = new();
                toAdd.city = group.Key;
                toAdd.total = 0;
                foreach (var city in group)
                {
                    toAdd.services.Add(city.services[0]);
                }
                var tempServices = toAdd.services.GroupBy(s => s.name);
                toAdd.services = new();
                foreach (var service in tempServices)
                {
                    ServiceData sToAdd = new ServiceData();
                    sToAdd.payers = new();
                    sToAdd.name = service.Key;
                    sToAdd.total = 0;
                    foreach (var serv in service)
                    {
                        sToAdd.payers.Add(serv.payers[0]);
                        sToAdd.total++;
                    }
                    toAdd.services.Add(sToAdd);
                }
                toAdd.total = toAdd.services.Count();
                resCityDatas.Add(toAdd);
            }

            return resCityDatas;
        }
    }
}