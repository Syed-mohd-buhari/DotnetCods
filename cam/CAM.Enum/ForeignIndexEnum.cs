namespace CAM.Enum
{
    public enum ForeignIndexStatus : int
    {
        DesignComponentOrphan = 0,
        SystemTypeOrphan,
        SimilarityGroups,
        GroupCorrection,
        Committed,
        Canceled
    }

    public enum ForeignIndexSource : int
    {
        NotSpecified = 0,
        MajorHardwareBuild,
        MajorSoftwareBuild,
        DesignComponentFamily
    }
}
