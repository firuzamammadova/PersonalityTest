using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalityTest.Domain.Entities;
public class UserAnswer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public Answer? Answer { get; set; }
}
