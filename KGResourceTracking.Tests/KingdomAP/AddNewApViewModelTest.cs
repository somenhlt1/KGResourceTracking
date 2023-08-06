using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KGResourceTracking.Data;
using KGResourceTracking.KingdomAP;

using Microsoft.EntityFrameworkCore;

namespace KGResourceTracking.Tests.KingdomAP
{
    public class AddNewApViewModelTest
    {
        [Fact]
        public async Task OnSubmit_WithValidData_AddNewApToDatabase()
        {
            //Given
            AutoMocker mocker = new();
            using var factory = mocker.WithDbScope();
            AddNewApViewModel vm = mocker.CreateInstance<AddNewApViewModel>();
            //When
            vm.PlayerId = "123456";
            vm.ApTypeChoice = ApTypeEnum.TwoHundred;

            await vm.SubmitCommand.ExecuteAsync(null);
            //Then
            using var assertContext = factory.CreateDbContext();
            ApType ap = await assertContext.ApTypes.SingleAsync();
            Assert.Equal("123456", ap.Id.ToString());
            Assert.Equal(ApTypeEnum.TwoHundred, ap.Type);
        }
    }
}
