using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.AttackTarget
{
    public enum BearingDefinition
    {
        Arbitrary = 0
    }

    public enum AbsoluteHeightDefinition
    {
        Arbitrary = 0,
        Maximum = 1
    }

    public enum VisibleHeightDefinition
    {
        Arbitrary = 0
    }

    public enum AbsoluteLengthDefinition
    {
        Arbitrary = 0,
        Maximum = 1
    }

    public enum VisibleLengthDefinition
    {
        Arbitrary = 0
    }

    public enum TargetRangeDefinition
    {
        Arbitrary = 0,
        ByVisibleHight = 1
    }

    public enum AoBDefinition
    {
        Arbitrary = 0,
        ByRangeAndVisibleLength = 1
    }

    public enum HullTimeDefinition
    {
        Arbitrary = 0
    }

    public enum OneDegreeTimeDefinition
    {
        Arbitrary = 0
    }

    public enum TargetSpeedDefinition
    {
        Arbitrary = 0,
        Linear = 1,
        Angular = 2,
        LinearConstantBoatVelocity = 3
    }

    public enum AngularSpeedDefinition
    {
        Arbitrary = 0,
        ByOneDegreeTime = 1
    }

    public enum BoatSpeedDefinition
    {
        Arbitrary = 0
    }

    public enum LeadAngleDefinition
    {
        Arbitrary = 0,
        ByAngularSpeed = 1
    }

    public enum SingleOptionDefinition
    {
        O = 0
    }
}
