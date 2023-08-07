using System;
using Battle;

namespace Battle.Actions
{
    public class NextTurnUseCase
    {
        private UnitOfWork _unitOfWork;

        public NextTurnUseCase(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


    }
}