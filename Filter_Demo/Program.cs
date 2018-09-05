using System.Collections.Generic;
using System.Linq;

namespace Filter_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var regulars = new List<Regulars>();

            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则1", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则2", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则3", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则4", AnalysisConditons = "xxxx" });
            regulars.Add(new Regulars() { RegularID = 1, RegularName = "规则5", AnalysisConditons = "xxxx" });

            //追加filter条件
            var filterList = new List<IFilter>{
                                              new RegularIDFilter(),
                                              new RegularNameFilter(),
                                              new RegularCondtionFilter()
                                            };

            var andCriteria = new AndFilter(filterList);

            //进行 And组合 过滤
            andCriteria.Filter(regulars);
        }
    }


    /// <summary>
    /// 各种催付规则
    /// </summary>
    public class Regulars
    {
        public int RegularID { get; set; }

        public string RegularName { get; set; }

        public string AnalysisConditons { get; set; }
    }

    public interface IFilter
    {
        List<Regulars> Filter(List<Regulars> regulars);
    }


    public class RegularIDFilter : IFilter
    {
        /// <summary>
        /// Regulars的过滤逻辑
        /// </summary>
        /// <param name="regulars"></param>
        /// <returns></returns>
        public List<Regulars> Filter(List<Regulars> regulars)
        {
            return null;
        }
    }

    public class RegularNameFilter : IFilter
    {
        /// <summary>
        /// regularName的过滤方式
        /// </summary>
        /// <param name="regulars"></param>
        /// <returns></returns>
        public List<Regulars> Filter(List<Regulars> regulars)
        {
            return null;
        }
    }


    public class RegularCondtionFilter : IFilter
    {
        /// <summary>
        /// Condition的过滤条件
        /// </summary>
        /// <param name="regulars"></param>
        /// <returns></returns>
        public List<Regulars> Filter(List<Regulars> regulars)
        {
            return null;
        }
    }

    /// <summary>
    /// filter的 And 模式
    /// </summary>
    public class AndFilter : IFilter
    {
        List<IFilter> filters = new List<IFilter>();

        public AndFilter(List<IFilter> filters)
        {
            this.filters = filters;
        }

        public List<Regulars> Filter(List<Regulars> regulars)
        {
            var regularlist = new List<Regulars>(regulars);

            foreach (var criteriaItem in filters)
            {
                regularlist = criteriaItem.Filter(regularlist);
            }

            return regularlist;
        }
    }

    public class OrFilter : IFilter
    {
        List<IFilter> filters = null;

        public OrFilter(List<IFilter> filters)
        {
            this.filters = filters;
        }

        public List<Regulars> Filter(List<Regulars> regulars)
        {
            //用hash去重
            var resultHash = new HashSet<Regulars>();

            foreach (var filter in filters)
            {
                var smallPersonList = filter.Filter(regulars);

                foreach (var small in smallPersonList)
                {
                    resultHash.Add(small);
                }
            }

            return resultHash.ToList();
        }
    }
}
