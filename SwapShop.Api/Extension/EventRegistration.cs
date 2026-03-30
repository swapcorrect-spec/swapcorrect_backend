using ProjectX.Application.CommandHandler.Auth;
using SwapShop.Application.CommandHandler;
using SwapShop.Application.CommandHandler.Auth;
using SwapShop.Application.CommandHandler.ListingItem;
using SwapShop.Application.CommandHandler.Payment;
using SwapShop.Application.CommandHandler.Report;
using SwapShop.Application.Commands.Report;

namespace SwapShop.Api.Extension
{
    public static class EventRegistration
    {
        public static void ConfigureEvent(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddMediatR(config =>
            {

                config.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(CloseSwapCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(DeleteUserCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(ConfirmEmailCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(ForgetPasswordCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(ResetPasswordCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(UpdateUserInfoCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(GoogleLoginCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(AdminReviewCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(CreateListItemCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(StartSwapCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(ReportUserCommand).Assembly);
                config.RegisterServicesFromAssembly(typeof(AdminReportNoteCommand).Assembly);
                config.RegisterServicesFromAssembly(typeof(ChangeReportStatusCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(IntializePaymentForPaystackCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(ConfirmPaystackPaymentCommandHandler).Assembly);
                config.RegisterServicesFromAssembly(typeof(CreateAccountOrUpdateCommandHandler).Assembly);
            });
        }
    }
}
