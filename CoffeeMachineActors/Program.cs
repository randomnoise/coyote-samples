﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.Coyote.Actors;

namespace Microsoft.Coyote.Samples.CoffeeMachineActors
{
    public static class Program
    {
        private static bool RunForever = false;

        public static void Main()
        {
            RunForever = true;
            IActorRuntime runtime = RuntimeFactory.Create(); // Configuration.Create().WithVerbosityEnabled());
            Execute(runtime);
            Console.ReadLine();
            Console.WriteLine("User cancelled the test by pressing ENTER");
        }

        private static void OnRuntimeFailure(Exception ex)
        {
            Console.WriteLine("Unhandled exception: {0}", ex.Message);
        }

        [Microsoft.Coyote.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime)
        {
            runtime.OnFailure += OnRuntimeFailure;
            runtime.RegisterMonitor<LivenessMonitor>();
            ActorId driver = runtime.CreateActor(typeof(FailoverDriver), new FailoverDriver.ConfigEvent(RunForever));
            runtime.SendEvent(driver, new FailoverDriver.StartTestEvent());
        }
    }
}
