using System;
using System.Collections.Generic;
using Battle.Common;

public class Result : ValueObject<string>
{
    public override string Value()
    {
        throw new NotImplementedException();
    }

    public List<Item> ObtainedLoots()
    {
        throw new NotImplementedException();
    }

    public int ObtainedMoney()
    {
        throw new NotImplementedException();
    }
}