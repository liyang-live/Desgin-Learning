using System;
using System.Collections.Generic;

namespace ConsoleApp38
{
    /// <summary>
    /// Demo类：测试程序
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            FilterManager filterManager = new FilterManager(new Target());
            filterManager.setFilter(new AuthenticationFilter());
            filterManager.setFilter(new DebugFilter());

            Client client = new Client();
            client.setFilterManager(filterManager);
            client.sendRequest("HOME");
        }
    }

    /// <summary>
    /// Filter接口：定义过滤器统一对外处理的接口函数
    /// </summary>
    public interface Filter
    {
        void execute(string request);
    }

    /// <summary>
    /// AuthenticationFilter类：实际的过滤器之一
    /// </summary>
    public class AuthenticationFilter : Filter
    {
        public void execute(string request)
        {
            Console.WriteLine("Authenticating request: " + request);
        }
    }

    /// <summary>
    /// DebugFilter类：实际的过滤器之二
    /// </summary>
    public class DebugFilter : Filter
    {
        public void execute(string request)
        {
            Console.WriteLine("Request log: " + request);
        }
    }

    /// <summary>
    /// FilterChain类：过滤器链
    /// </summary>
    public class FilterChain
    {

        private List<Filter> filters = new List<Filter>();
        private Target target;

        public void addFilter(Filter filter)
        {
            filters.Add(filter);
        }

        public void execute(string request)
        {
            foreach (Filter filter in filters)
            {
                filter.execute(request);
            }
            target.execute(request);
        }

        public void setTarget(Target target)
        {
            this.target = target;
        }
    }

    /// <summary>
    /// FilterManager类：过滤器管理类
    /// </summary>
    public class FilterManager
    {
        FilterChain filterChain;

        public FilterManager(Target target)
        {
            filterChain = new FilterChain();
            filterChain.setTarget(target);
        }

        public void setFilter(Filter filter)
        {
            filterChain.addFilter(filter);
        }

        public void filterRequest(string request)
        {
            filterChain.execute(request);
        }
    }

    /// <summary>
    /// Target类：请求的实际处理类
    /// </summary>
    public class Target
    {
        public void execute(string request)
        {
            Console.WriteLine("Executing request: " + request);
        }
    }

    /// <summary>
    /// Client类：客户端程序
    /// </summary>
    public class Client
    {
        FilterManager filterManager;

        public void setFilterManager(FilterManager filterManager)
        {
            this.filterManager = filterManager;
        }

        public void sendRequest(string request)
        {
            filterManager.filterRequest(request);
        }
    }
}
