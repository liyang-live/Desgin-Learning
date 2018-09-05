using System;
using System.Collections.Generic;
using System.Linq;

namespace pipeline_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Goods> glist = new List<Goods>()
                    {
                       new Goods(){ Name="雕牌洗衣粉 中", Price=19},
                       new Goods(){ Name="黑人牙膏", Price=16},
                       new Goods(){ Name="云南白药牙膏", Price=35},
                       new Goods(){ Name="康师傅绿茶", Price=3},
                       new Goods(){ Name="洗发露", Price=42},
                       new Goods(){ Name="铁观音 小袋", Price=15},
                    };


            //过滤器合成
            AndFilter filterCombination = new AndFilter();
            AndFilter andFilter = new AndFilter();
            //自定义过滤器
            var priceFilter = new GoodsPriceRangeFilter(15, 20);
            var nameFilter = new GoodsNameFilter("牙膏");
            //and条件过滤数据集
            andFilter.AddFilter(priceFilter);
            andFilter.AddFilter(nameFilter);
            //组合and过滤器
            filterCombination.AddFilter(andFilter);
            //创建管道 根据过滤器来过滤数据集
            Pipe p = new Pipe();
            List<Goods> filteredGoods = p.Filter(glist, filterCombination);
            filteredGoods.ForEach(g =>
            {
                Console.WriteLine(g.Name + "..." + g.Price + "元");
            });

        }
    }

    //定义过滤器接口
    public interface IFilter
    {
        //数据是否匹配
        bool IsMatch(Goods goods);
    }

    /// <summary>
    /// 过滤器合成抽象类(组合过滤条件)
    /// </summary>
    public abstract class CompositeFilter : IFilter
    {
        protected List<IFilter> filters = new List<IFilter>();
        public abstract bool IsMatch(Goods goods);
        public void AddFilter(IFilter filter)
        {
            this.filters.Add(filter);
        }
        public void AddFilterRange(List<IFilter> filters)
        {
            this.filters.AddRange(filters);
        }
    }

    //And过滤器(条件)
    public class AndFilter : CompositeFilter
    {
        //重写匹配方法,交由具体的过滤去具体实现
        public override bool IsMatch(Goods goods)
        {
            return filters.All(filter => filter.IsMatch(goods));
        }
    }

    //Or过滤器
    public class OrFilter : CompositeFilter
    {
        public override bool IsMatch(Goods goods)
        {
            return filters.Any(filter => filter.IsMatch(goods));
        }
    }


    public class Pipe
    {
        private IFilter filter;
        public List<Goods> Goods { get; private set; }      //原数据
        public List<Goods> newGoods { get; private set; }   //过滤产生的最终数据
        public List<Goods> Filter(List<Goods> goods, IFilter filter)
        {
            this.newGoods = new List<Goods>();
            this.Goods = goods;
            this.filter = filter;
            DoFilter();
            return newGoods;
        }
        //过滤操作
        private void DoFilter()
        {
            //对每个商品进行具体的匹配操作，不符合的过滤掉
            foreach (var good in Goods.Where(g => filter.IsMatch(g)))
            {
                this.newGoods.Add(good);
            }
        }
    }

    //名称过滤
    public class GoodsNameFilter : IFilter
    {
        private string name;
        public GoodsNameFilter(string name)
        {
            this.name = name;
        }

        public bool IsMatch(Goods goods)
        {
            if (goods.Name.Contains(name))
                return true;
            return false;
        }
    }
    //价格区间过滤
    public class GoodsPriceRangeFilter : IFilter
    {
        int min;
        int max;
        public GoodsPriceRangeFilter(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        public bool IsMatch(Goods goods)
        {
            if (goods.Price >= min && goods.Price <= max)
                return true;
            return false;
        }
    }

    public class Goods
    {
        public string Name { get; set; }

        public int Price { get; set; }
    }
}
