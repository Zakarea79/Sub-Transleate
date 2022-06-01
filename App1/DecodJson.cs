using System.Collections.Generic;

namespace sub_Transleator_x 
{
    public class SentencesItem
    {
        public string trans { get; set; }
        public string orig { get; set; }
        public int backend { get; set; }
    }

    public class EntryItem
    {
        public string word { get; set; }
        public List<string> reverse_translation { get; set; }
        public double score { get; set; }
    }

    public class DictItem
    {
        public string pos { get; set; }
        public List<string> terms { get; set; }
        public List<EntryItem> entry { get; set; }
        public string base_form { get; set; }
        public int pos_enum { get; set; }
    }

    public class Spell
    {
    }

    public class Ld_result
    {
        public List<string> srclangs { get; set; }
        public List<double> srclangs_confidences { get; set; }
        public List<string> extended_srclangs { get; set; }
    }

    public class Root
    {
        public List<SentencesItem> sentences { get; set; }
        public List<DictItem> dict { get; set; }
        public string src { get; set; }
        public double confidence { get; set; }
        public Spell spell { get; set; }
        public Ld_result ld_result { get; set; }
    }

}