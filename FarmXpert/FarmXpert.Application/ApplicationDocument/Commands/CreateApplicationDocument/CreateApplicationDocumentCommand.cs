using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmXpert.Application.ApplicationDocument.Commands.CreateApplicationDocument;

public record CreateApplicationDocumentCommand(string Title, Stream FileStream, string FileExtension, string OwnerId) : IRequest<FarmXpert.Domain.Entities.ApplicationDocument>;