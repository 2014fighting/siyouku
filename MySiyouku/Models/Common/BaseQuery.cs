using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySiyouku.Models.Common
{
    public class BaseQuery
    { 
            /// <summary>
            /// 页码
            /// </summary>
            private int _page = 1;

            public int Page
            {
                get
                {
                    if (_page <= 1) return 1;
                    return _page;
                }
                set
                {
                    _page = value;
                }
            }

            /// <summary>
            /// 条数
            /// </summary>
            private int _rows = 10;

            public int Rows
            {
                get
                {
                    if (_rows <= 1) return 10;
                    return _rows;
                }
                set
                {
                    _rows = value;
                }
            }

            /// <summary>
            /// 排序方式
            /// </summary>
            private string _order = "desc";
            public string Order { get { return _order; } set { _order = value; } }

            /// <summary>
            /// 排序字段
            /// </summary>
            public string Sort { get; set; }

            private int _total;

            public int Total
            {
                get
                {
                    if (_total < 0) return 0;
                    else return _total;
                }
                set { _total = value; }
            }
 
    }
}