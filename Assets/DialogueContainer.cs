using System.Xml.Serialization;
using System;

namespace DialogueContainer
{
    
    [Serializable, XmlRoot("Dialogue")]
    public class Dialogue
    {
        [XmlElement("Node")]
        public Node[] nodes;
    }

    [Serializable]
    public class Node
    {
        [XmlAttribute("Id")]
        public int id;
        [XmlElement("Text")]
        public string text;
        [XmlElement("Option")]
        public Option[] options;
    }
    
    [Serializable]
    public class Option
    {
        [XmlAttribute("ToNodeId")]
        public int id;
        [XmlText]
        public string text;
    }
}
