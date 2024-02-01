using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tekton.Domain.Entities;
public partial class Status : EntityBase<byte>
{
    public string Name { get; set; } = "";
}

