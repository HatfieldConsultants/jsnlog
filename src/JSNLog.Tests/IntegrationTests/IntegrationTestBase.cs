﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;

namespace JSNLog.Tests.IntegrationTests
{
    public class IntegrationTestBaseContext : IDisposable
    {
        public IWebDriver Driver
        {
            get; private set;
        }

        // The port 5000 is always used by kestrel
        private const string _baseUrl = "http://localhost:5000";

        public IntegrationTestBaseContext()
        {
            // To use ChromeDriver, you must have chromedriver.exe. Download from
            // https://sites.google.com/a/chromium.org/chromedriver/downloads

            //TODO: fix hard coding of path to dependencies folder
            // The following code works fine in .Net 40, but in DNX451 executingAssemblyLocation is set to "".
            // So hard code the path for now.
            //var executingAssembly = Assembly.GetExecutingAssembly();
            //var executingAssemblyLocation = executingAssembly.Location;
            //string assemblyFolder = Path.GetDirectoryName(executingAssemblyLocation);
            //string dependenciesFolder = Path.Combine(assemblyFolder, "Dependencies");

            string dependenciesFolder = @"D:\Dev\JSNLog\jsnlog\src\JSNLog.Tests\IntegrationTests\Dependencies";
            Driver = new ChromeDriver(dependenciesFolder);
        }

        public void Dispose()
        {
            // Close the browser if there is no error. Otherwise leave open.
            if (!ErrorOnPage())
            {
                Driver.Quit();
            }
        }

        public void OpenPage(string relativeUrl)
        {
            string absoluteUrl = _baseUrl + relativeUrl;
            Driver.Navigate().GoToUrl(absoluteUrl);
        }

        /// <summary>
        /// Returns true if there is an error element on the page, or if the "test running" message is still on the page
        /// (meaning the test js crashed).
        /// </summary>
        /// <returns></returns>
        public bool ErrorOnPage()
        {
            // Check for C# exception
            bool unhandledExceptionOccurred = Driver.PageSource.Contains("An unhandled exception occurred");
            bool noConnection = Driver.PageSource.Contains("ERR_CONNECTION_REFUSED");

            if (unhandledExceptionOccurred || noConnection)
            {
                return true;
            }

            try
            {
                // Throws NoSuchElementException if error-occurred not found
                Driver.FindElement(By.ClassName("error-occurred"));
            }
            catch (NoSuchElementException)
            {
                try
                {
                    // Throws NoSuchElementException if running not found
                    Driver.FindElement(By.Id("running"));
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class IntegrationTestBase : IClassFixture<IntegrationTestBaseContext>
    {
        IntegrationTestBaseContext _context;

        // Runs before each test runs
        public IntegrationTestBase(IntegrationTestBaseContext context)
        {
            _context = context;
        }

        // Runs after each test has run
        public void Dispose()
        {
        }

        protected IWebDriver Driver { get { return _context.Driver; } }

        public bool ErrorOnPage()
        {
            return _context.ErrorOnPage();
        }

        public void OpenPage(string relativeUrl)
        {
            _context.OpenPage(relativeUrl);
        }
    }
}

