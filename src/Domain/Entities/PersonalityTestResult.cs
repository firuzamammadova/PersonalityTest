using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalityTest.Domain.Entities;
public class PersonalityTestResult
{
    public int Id { get; set; }
    public string? PersonalityTrait { get; set; } 
    public DateTime TestTakenOn { get; set; }
}
