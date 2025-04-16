using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAPIPerguntas.Models
{
    public class Pergunta
    {
        public int id { get; set; }
        public string pergunta { get; set; }

    }
    public class RootObject
    {
        public ObservableCollection<Pergunta> Perguntas { get; set; }
    }
}
