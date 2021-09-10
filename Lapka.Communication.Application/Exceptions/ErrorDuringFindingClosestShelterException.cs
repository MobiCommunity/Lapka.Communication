namespace Lapka.Communication.Application.Exceptions
{
    public class ErrorDuringFindingClosestShelterException : AppException
    {
        public ErrorDuringFindingClosestShelterException() : base($"Error occured during finding closest shelter ")
        {
        }

        public override string Code => "error_during_finding_closest_shelter";
    }
}