using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

using JuanMartin.Api.Utilities;
using System;
using JuanMartin.Models;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace JuanMartin.Api.Controllers
{
    public class ProblemsController : ApiController
    {
        // should be a GET, but to pass a request payload, so got to use a post
        [HttpPost]
        // POST: api/problems
        public HttpResponseMessage GetAnswers([FromBody] Exam euler)
        {

            //            Func<int, Problem> GetProblem = (id) =>
            //            {
            //                var problems = JuanMartin.Utilities.Euler.UtilityEulerProjectSolver.problems;
            //
            //                foreach (var problem in problems)
            //                {
            //                    if (problem.Id == id)
            //                        return problem;
            //                }
            //
            //                return null;
            //            };

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return UtilityHttp.ResponseBadRequest(Request);
            }
            else
            {
                var results = new List<Result>();
                var stopWatch = new Stopwatch();

                if (euler != null)
                {
                    foreach (Problem p in euler.Problems)
                    {
                        if (p.Id > 0)
                        {
                            Result r = null;
                            try
                            {
                                var problem = JuanMartin.Utilities.Euler.UtilityEulerProjectSolver.GetProblemById(p.Id);

                                stopWatch.Restart();
                                if (problem != null)
                                    r = problem.Script(p);
                                else
                                    return UtilityHttp.ResponseBadRequest(Request, string.Format("Problem {0} has not been defined yet, please review problem id's in request and remove the problem section for this id.", p.Id));
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                return UtilityHttp.ResponseBadRequest(Request, string.Format("Probably problem {0} has not been implemented yet: {1}", p.Id, e.Message));
                            }
                            catch (Exception e)
                            {
                                return UtilityHttp.ResponseBadRequest(Request, e.Message);
                            }
                            finally
                            {
                                stopWatch.Stop();

                                if (r != null)
                                {
                                    r.Duration = stopWatch.Elapsed.TotalMilliseconds;

                                    results.Add(r);
                                }
                            }
                        }
                    }
                }
                if (results.Any())
                {
                    return UtilityHttp.ResponseOk(Request, results);
                }
                else
                    return UtilityHttp.ResponseNoProblem(Request);
            }
        }

        [HttpGet]
        // GET: api/problems/{id}?arguments
        public HttpResponseMessage GetAnswer(int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return UtilityHttp.ResponseBadRequest(Request);
            }
            else
            {
                var results = new List<Result>();
                var stopWatch = new Stopwatch();

                if (id > 0)
                {
                    Result r = null;
                    try
                    {
                        stopWatch.Restart();
                        var problem = JuanMartin.Utilities.Euler.UtilityEulerProjectSolver.GetProblemById(id);

                        if (problem != null)
                        {
                            // querystring mapping
                            foreach (var param in Request.GetQueryNameValuePairs())
                            {
                                switch (param.Key)
                                {
                                    case "intnumber":
                                        {
                                            problem.IntNumber = Convert.ToInt32(param.Value);
                                            break;
                                        }
                                    case "longnumber":
                                        {
                                            problem.LongNumber = Convert.ToInt64(param.Value);
                                            break;
                                        }
                                    case "numbers":
                                        {
                                            problem.Numbers = param.Value.Split('_').Select(c => Convert.ToInt64(c)).ToArray();
                                            break;
                                        }
                                    case "listofnumbers":
                                        {
                                            problem.ListOfNumbers = param.Value.Split('_').Select(c => Convert.ToInt32(c)).ToList<int>();
                                            break;
                                        }
                                }
                            }
                            r = problem.Script(problem);
                        }
                        else
                            return UtilityHttp.ResponseBadRequest(Request, string.Format("Problem {0} has not been defined yet, please review problem id's in request and remove the problem section for this id.", id));
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        return UtilityHttp.ResponseBadRequest(Request, string.Format("Probably problem {0} has not been implemented yet: {1}", id, e.Message));
                    }
                    catch (Exception e)
                    {
                        return UtilityHttp.ResponseBadRequest(Request, e.Message);
                    }
                    finally
                    {
                        stopWatch.Stop();

                        if (r != null)
                        {
                            r.Duration = stopWatch.Elapsed.TotalMilliseconds;

                            results.Add(r);
                        }
                    }
                }
                else
                    return UtilityHttp.ResponseBadRequest(Request, string.Format("Problem {0} has not been defined yet, please review problem id's in request and remove the problem section for this id.", id));

                if (results.Any())
                {
                    return UtilityHttp.ResponseOk(Request, results);
                }
                else
                    return UtilityHttp.ResponseNoProblem(Request);
            }
        }

            }
}