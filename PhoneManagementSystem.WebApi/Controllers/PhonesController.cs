namespace PhoneManagementSystem.WebApi.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Net;

    using LinqKit;

    using PhoneManagementSystem.Commons;
    using PhoneManagementSystem.Data;
    using PhoneManagementSystem.Models;
    using PhoneManagementSystem.WebApi.BindingModels;
    using PhoneManagementSystem.WebApi.DataModels;
    using PhoneManagementSystem.WebApi.Providers;
    using PhoneManagementSystem.WebApi.BindingModels.Admin;

    [RoutePrefix("api/Phones")]
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class PhonesController : BaseApiController
    {
        public const string ModelName = "phone";

        public PhonesController()
            : base(new PhoneSystemData())
        {
        }

        public PhonesController(IPhoneSystemData data)
            : base(data)
        {
        }

        [HttpGet]
        public IHttpActionResult GetPhones([FromUri]AdminPhoneBindinModel model)
        {
            if (model == null)
            {
                model = new AdminPhoneBindinModel();
            }

            var phones = this.Data.Phones.All();

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                phones = phones
                    .Where(p => p.PhoneId.Contains(model.Search));

            }

            if (model.IsAvailable.HasValue)
            {
                if (model.IsAvailable.Value)
                {
                    phones = phones
                        .Where(p => p.AvailableAfter.Value < DateTime.Now)
                        .Where(p => p.PhoneStatus == PhoneStatus.Free);
                }
                else
                {
                    phones = phones
                        .Where(p => p.AvailableAfter.Value > DateTime.Now || !p.AvailableAfter.HasValue)
                        .Where(p => p.PhoneStatus != PhoneStatus.Free);
                }

            }

            var phonesToReturn = phones.Select(p => new
            {
                p.PhoneId,
                PhoneStatus = p.PhoneStatus.ToString(),
                p.AvailableAfter
            });

            return Ok(phonesToReturn);
        }

        /// <summary>
        /// Get phones with filter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(PhoneWtihUserInfoDataModel))]
        [Route("UserInfo")]
        public IHttpActionResult GetPhonesWithUserInfo([FromUri]AdminPhonesFilterBindinModel model)
        {
            if (model == null)
            {
                model = new AdminPhonesFilterBindinModel();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var phones = this.Data.Phones.All();

            if (!string.IsNullOrWhiteSpace(model.PhoneStatus))
            {
                var phoneStatuses = model.PhoneStatus.Split('|');
                PhoneStatus status;

                // http://stackoverflow.com/questions/782339/how-to-dynamically-add-or-operator-to-where-clause-in-linq
                var searchPredicate = PredicateBuilder.False<Phone>();

                foreach (var phoneStatus in phoneStatuses)
                {
                    if (Enum.TryParse<PhoneStatus>(phoneStatus, out status))
                    {
                        var closureVariable = status;
                        searchPredicate =
                          searchPredicate.Or(p => p.PhoneStatus == closureVariable);
                    }
                    else
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid phone status." });
                    }
                }

                phones = phones.AsExpandable().Where(searchPredicate);
            }

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                phones = phones
                    .Where(p => p.PhoneId.Contains(model.Search));

            }

            if (model.HasRouming.HasValue)
            {
                phones = phones.Where(p => p.HasRouming == model.HasRouming.Value);
            }

            if (!string.IsNullOrWhiteSpace(model.CreditLimit))
            {
                if (model.CreditLimit.Contains('-'))
                {
                    var strings = model.CreditLimit.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strings.Count() != 2)
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid credit limit filter." });
                    }

                    try
                    {
                        var min = int.Parse(strings[0]);
                        var max = int.Parse(strings[1]);
                        phones = phones.Where(p => p.CreditLimit >= min).Where(p => p.CreditLimit <= max);
                    }
                    catch (Exception)
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid credit limit filter." });
                    }
                }
                else if (model.CreditLimit.Contains('<'))
                {
                    var strings = model.CreditLimit.Split(new char[] { '<', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strings.Count() != 1)
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid credit limit filter." });
                    }

                    try
                    {
                        var max = int.Parse(strings[0]);
                        phones = phones.Where(p => p.CreditLimit < max);
                    }
                    catch (Exception)
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid credit limit filter." });
                    }
                }
                else if (model.CreditLimit.Contains('>'))
                {
                    var strings = model.CreditLimit.Split(new char[] { '>', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strings.Count() != 1)
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid credit limit filter." });
                    }

                    try
                    {
                        var min = int.Parse(strings[0]);
                        phones = phones.Where(p => p.CreditLimit > min);
                    }
                    catch (Exception)
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid credit limit filter." });
                    }
                }
                else
                {
                    return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid credit limit filter." });
                }
            }

            if (!string.IsNullOrWhiteSpace(model.CardType))
            {
                var cardTypes = model.CardType.Split('|');
                CardType type;

                foreach (var cardType in cardTypes)
                {
                    if (Enum.TryParse<CardType>(cardType, out type))
                    {
                        phones = phones
                            .Where(p => p.CardType == type)
                            .Union(phones.Where(p => p.CardType == type));
                    }
                    else
                    {
                        return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid card type." });
                    }
                }
            }

            if (model.IsAvailable.HasValue)
            {
                if (model.IsAvailable.Value)
                {
                    phones = phones
                        .Where(p => p.AvailableAfter.Value < DateTime.Now)
                        .Where(p => p.PhoneStatus == PhoneStatus.Free);
                }
                else
                {
                    phones = phones
                        .Where(p => p.AvailableAfter.Value > DateTime.Now || !p.AvailableAfter.HasValue)
                        .Where(p => p.PhoneStatus != PhoneStatus.Free);
                }

            }

            return this.OKWithPagingAndFilter(model, phones, PhoneWtihUserInfoDataModel.FromPhone);
        }

        [HttpPost]
        public IHttpActionResult AddPhone(PhoneBindingModel model)
        {
            this.CheckModelForNull(model, ModelName);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (this.Data.Phones.All().Any(p => p.PhoneId == model.PhoneId))
            {
                return this.Content(HttpStatusCode.Conflict, ReturnMessageProvider.ErrorDublicate(model.PhoneId, ModelName));
            }

            CardType cardType;

            var phone = new Phone()
            {
                CreditLimit = model.CreditLimit,
                HasRouming = false,
                PhoneId = model.PhoneId,
                PhoneStatus = PhoneStatus.Free,
                CreatedAt = DateTime.Now,
                CardType = CardType.Unknown
            };

            if (!string.IsNullOrWhiteSpace(model.CardType))
            {
                if (Enum.TryParse<CardType>(model.CardType, out cardType))
                {
                    phone.CardType = cardType;
                }
                else
                {
                    return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid card type." });
                }
            }
            else
            {
                model.CardType = CardType.Unknown.ToString();
            }

            if (model.HasRouming.HasValue)
            {
                phone.HasRouming = model.HasRouming.Value;
            }
            else
            {
                model.HasRouming = false;
            }

            try
            {
                this.Data.Phones.Add(phone);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {
                this.GetExceptionMessage(ex);
            }

            return this.CreatedAtRoute(GlobalConstants.DefaultApi, new { Id = phone.PhoneId }, model);
        }

        [HttpPut]
        public IHttpActionResult EditPhone(string id, PhoneBindingModel model)
        {
            this.CheckModelForNull(model, ModelName);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var phone = this.Data.Phones.GetById(id);
            this.CheckObjectForNull(phone, ModelName, id);

            if (model.HasRouming.HasValue)
            {
                phone.HasRouming = model.HasRouming.Value;
            }

            CardType cardType;
            if (!string.IsNullOrWhiteSpace(model.CardType))
            {
                if (Enum.TryParse<CardType>(model.CardType, out cardType))
                {
                    phone.CardType = cardType;
                }
                else
                {
                    return this.Content(HttpStatusCode.BadRequest, new { Message = "Invalid card type." });
                }
            }

            if (model.HasRouming.HasValue)
            {
                phone.HasRouming = model.HasRouming.Value;
            }

            if (model.CreditLimit.HasValue)
            {
                phone.CreditLimit = model.CreditLimit.Value;
            }

            try
            {
                this.Data.Phones.Update(phone);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {
                this.GetExceptionMessage(ex);
            }

            return Ok(ReturnMessageProvider.SuccessEdit(id, ModelName));
        }

        [HttpDelete]
        public IHttpActionResult DeletePhone(string id)
        {
            var phone = this.Data.Phones.GetById(id);
            this.CheckObjectForNull(phone, ModelName, id);

            try
            {
                this.Data.Phones.Delete(phone);
                this.Data.SaveChanges();
            }
            catch (Exception ex)
            {
                this.GetExceptionMessage(ex);
            }

            return this.Ok(ReturnMessageProvider.SuccessDelete(id, ModelName));
        }
    }
}
