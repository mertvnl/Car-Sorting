using System;

namespace CustomEventSystem
{
    public static class Events
    {
        public static readonly Event<GateType> OnGateButtonPressed = new Event<GateType>();
    }
}