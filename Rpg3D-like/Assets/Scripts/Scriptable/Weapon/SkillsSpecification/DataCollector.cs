using System.Text;

namespace Scriptable.Weapon.SkillsSpecification
{
    public class DataCollector
    {
        public StringBuilder SkillData = new StringBuilder();

        public DataCollector(string specification)
        {
            SkillData.Append(specification);
        }

        public void AddDataFromNewLine(string optionalSpecification)
        {
            SkillData.Append("\n");
            SkillData.Append(optionalSpecification);
        }
        
        public void AddDataInSameLine(string optionalSpecification)
        {
            SkillData.Append(" =>  ");
            SkillData.Append(optionalSpecification);
        }
    }
}