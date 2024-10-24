using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Model
{
    public class UiOption
    {
        public static Dictionary<UI_STATUS, UI_COLOR_DEF> UI_STATUS_COLOR_DEF = new Dictionary<UI_STATUS, UI_COLOR_DEF>
        {
            { UI_STATUS.NONE, new UI_COLOR_DEF{Back= Color.White,Fore=Color.Black }},
            { UI_STATUS.ING, new UI_COLOR_DEF{Back= Color.Orange,Fore=Color.White }},
            { UI_STATUS.OK, new UI_COLOR_DEF{Back= Color.Green,Fore=Color.White }},
            { UI_STATUS.FAIL, new UI_COLOR_DEF{Back= Color.Red,Fore=Color.White }},
        };

        public class UI_COLOR_DEF
        {
            public Color Fore { get; set; }
            public Color Back { get; set; }
        }


        public enum UI_STATUS
        {
            NONE,
            ING,
            OK,
            FAIL,
        }

    }
}
