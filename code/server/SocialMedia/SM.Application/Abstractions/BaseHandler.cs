using Microsoft.EntityFrameworkCore;
using SM.Application.Behaviors;
using SM.Application.Database;

namespace SM.Application.Abstractions;

internal class BaseHandler(IDbContextFactory<ApplicationDbContext> contextFactory, ILoggingBehaviour log) { }
