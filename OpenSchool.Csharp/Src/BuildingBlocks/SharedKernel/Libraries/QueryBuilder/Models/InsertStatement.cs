﻿namespace SharedKernel.Libraries.QueryBuilder;

public class InsertStatement
{
    public string Statement { get; set; }

    public Dictionary<string, object> Parameter { get; set; } = new Dictionary<string, object>();
}