using System;
using System.Collections.Generic;

namespace Common.Models;

public record RefineItem(string Title, IEnumerable<RefineField> Fields);

public record RefineField(string Name, bool Active, double Boost); 