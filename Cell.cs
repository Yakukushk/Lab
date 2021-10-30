using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace LabCalculator1
{
    [Serializable()]
    public class Cell : DataGridViewTextBoxCell
    {
        private string expression;
        private DataGridViewCell _parent;
        private string form;
        private string _name;
        public string index { get; private set; }

        private HashSet<string> variables = new HashSet<string>();

        public HashSet<string> Variables
        {
            get { return variables; }
        }

        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }
        

       
        public DataGridViewCell parent {
            get { return _parent; }
            
            
        }
        public string Name
        {
            get { return _name; }

        }
        public List<Cell> referencesFromThis = new List<Cell>();
        public List<Cell> pointersToThis = new List<Cell>();
        public Cell(DataGridViewCell parent, string name, string _expression, string _index, List<Cell> reference, List<Cell> pointers) {
            _parent = parent;
            _name = name;
            expression = _expression;
            _index = index;
            this.referencesFromThis.Clear();
            this.referencesFromThis.AddRange(reference);
            this.pointersToThis.Clear();
            this.pointersToThis.AddRange(pointers);
        }

    }
}
