public class MultipleContainersDecoder : PackingVectorUsingPartDecoder<ContainerProperties>
{
    public MultipleContainersDecoder(IReadOnlyList<ContainerProperties> containers) : base(containers) { }
}

public class OneContainerDecoder : PackingVectorNonUsingPartDecoder<ContainerProperties>
{
    public OneContainerDecoder(ContainerProperties container) : base(container) { }
}