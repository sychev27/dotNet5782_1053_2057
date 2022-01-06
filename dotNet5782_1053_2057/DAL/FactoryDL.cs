public static class FactoryDL
{
    public static DalXml.IDal GetDL(string libraryType)
    {
        if (libraryType == "a")
            return DalXml.DalXml1.Instance;
        else if (libraryType == "b")
            return DalObject.DalApi.DataSource.Instance;
        else
            throw new DalXml.DO.EXItemNotFoundException();
    }
}


