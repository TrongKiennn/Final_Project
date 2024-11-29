using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoListBinding1610.Helper {
    public static class SieuCapVoDichKhongCanBiet {
        public static bool IsEmpty(this string value) {
            return value.Length == 0;
        }

        public static double m (this double value) {
            return value;
        }

        // 5.mm() = 0.005m 
        public static double mm(this double value) {
            return value / 1000;
        }
    }
}
