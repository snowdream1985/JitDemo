﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace VirtualMethodsDispatching
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    [Config(typeof(Config))]
    public class VirtualMethodBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                //Add(Job.LegacyJitX86.With(Mode.SingleRun).WithLaunchCount(4).WithWarmupCount(4).WithTargetCount(4));
                //Add(Job.LegacyJitX64.With(Mode.SingleRun).WithLaunchCount(4).WithWarmupCount(4).WithTargetCount(4));
                Add(Job.RyuJitX64.With(Mode.SingleRun).WithLaunchCount(4).WithWarmupCount(4).WithTargetCount(4));
            }
        }

        // just for Turbo Boost
        [Benchmark(Baseline = true, Description = "non-virtual, pre-boot")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallBoot()
        {
            var plain = new Plain();
            for (int i = 0; i < Count; i++)
            {
                plain.Print();
            }
        }

        [Params(1000000000)]
        public int Count { get; set; }

        [Benchmark(Description = "virtual (base-derived)")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallDerived()
        {
            Base derived = new Derived();
            for (int i = 0; i < Count; i++)
            {
                derived.Print();
            }
        }

        [Benchmark(Description = "non-virtual")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallNonVirtual()
        {
            Base derived = new Derived();
            for (int i = 0; i < Count; i++)
            {
                derived.Print();
            }
        }
    }

    abstract class Base
    {
        public abstract void Print();
    }

    class Derived : Base
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override void Print()
        {

        }
    }

    class PlainBase
    {
         
    }

    class Plain : PlainBase
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Print()
        {

        }
    }
}