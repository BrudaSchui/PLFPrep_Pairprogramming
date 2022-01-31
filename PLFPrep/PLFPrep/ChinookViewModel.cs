using ChinookDbLib;

namespace PLFPrep
{
    internal class ChinookViewModel
    {
        private ChinookContext _db;

        public ChinookViewModel(ChinookContext db)
        {
            _db = db;
        }


    }
}
