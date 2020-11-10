using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleReader
{
    public class Srt
    {
        int _id;
        float _start;
        float _end;
        string _content;
        Boolean _canPrint;

        public int Id { get => _id; }
        public float Start { get => _start; }
        public float End { get => _end; }
        public string Content { get => _content; }

        public Boolean CanPrint { get => _canPrint; set { _canPrint = value; } }

        public Srt(int m_id, float m_start, float m_end, string m_content)
        {
            _id = m_id;
            _start = m_start;
            _end = m_end;
            _content = m_content;
            _canPrint = true;
        }
    }
}
