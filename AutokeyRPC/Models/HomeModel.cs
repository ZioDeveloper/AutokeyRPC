using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutokeyRPC.Models
{
    public class HomeModel
    {
        public IEnumerable<RPC_Cantieri> RPC_Cantieri { get; set; }   

        public IEnumerable<AUK_cantieri> AUK_cantieri { get; set; }

        public IEnumerable<RPC_Cantieri_vw> RPC_Cantieri_vw { get; set; }

        public IEnumerable<RPC_Telai> RPC_Telai { get; set; }

        public IEnumerable<RPC_telai_vw> RPC_Telai_vw { get; set; }

        public IEnumerable<RPC_FotoXTelaio> RPC_FotoXTelaio { get; set; }


    }
}