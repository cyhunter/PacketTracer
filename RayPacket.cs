using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;

// Size is too large to treat as HVA, so define RayPacket as class
internal class RayPacket
{
    public VectorPacket Starts {get; private set;}
    public VectorPacket Dirs {get; private set;}

    public RayPacket(VectorPacket _starts, VectorPacket _dirs)
    {
        Starts = _starts;
        Dirs = _dirs;
    }
}