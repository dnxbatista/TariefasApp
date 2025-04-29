using LiteDB;

namespace TariefasWebApi.Data
{
    public class TariefasDBContext
    {
        public static LiteDatabase Context = new LiteDatabase("Tariefas.db");
    }
}
