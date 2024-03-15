﻿using Skyline.DataMiner.Utils.Security.SecureIO;

namespace Skyline.DataMiner.Utils.SecureCoding.Tests
{
    [TestClass]
    public class SecurePathUnitTests
    {
        [TestMethod]
        // straightforward cases
        [DataRow(@"C:\skyline dataminer\", @"test.txt", @"C:\skyline dataminer\test.txt")]
        [DataRow(@"C:\skyline dataminer", @"test.txt", @"C:\skyline dataminer\test.txt")]
        // edge case where we traverse out of a subdir
        [DataRow(@"C:\skyline dataminer\", @"subdir\..\test.txt", @"C:\skyline dataminer\test.txt")]
        [DataRow(@"C:\skyline dataminer", @"subdir\..\test.txt", @"C:\skyline dataminer\test.txt")]
        [DataRow(@"C:\", @"subdir\..\..\test.txt", @"C:\test.txt")]
        // edge case where filename is an absolute path but point to the expected location
        [DataRow(@"C:\skyline dataminer\", @"C:\skyline dataminer\test.txt", @"C:\skyline dataminer\test.txt")]
        // network paths
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer\", @"test.txt", @"\\127.0.0.1\c$\skyline dataminer\test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer", @"test.txt", @"\\127.0.0.1\c$\skyline dataminer\test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer\", @"subdir\..\test.txt", @"\\127.0.0.1\c$\skyline dataminer\test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer", @"subdir\..\test.txt", @"\\127.0.0.1\c$\skyline dataminer\test.txt")]
        [DataRow(@"\\127.0.0.1\c$\", @"subdir\..\..\test.txt", @"\\127.0.0.1\c$\test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer\", @"\\127.0.0.1\c$\skyline dataminer\test.txt", @"\\127.0.0.1\c$\skyline dataminer\test.txt")]
        public void ConstructSecurePathSuccess(string basePath, string filename, string expectedResult)
        {
            string result = SecurePath.ConstructSecurePath(basePath, filename);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        // straightforward cases
        [DataRow(@"C:\skyline dataminer\", @"subdir\test.txt", @"C:\skyline dataminer\subdir\test.txt")]
        [DataRow(@"C:\skyline dataminer", @"subdir\test.txt", @"C:\skyline dataminer\subdir\test.txt")]
        // edge case where filename is an absolute path but point to the expected location
        [DataRow(@"C:\skyline dataminer", @"C:\skyline dataminer\subdir1\test.txt", @"C:\skyline dataminer\subdir1\test.txt")]
        // network paths
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer\", @"subdir\test.txt", @"\\127.0.0.1\c$\skyline dataminer\subdir\test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer", @"subdir\test.txt", @"\\127.0.0.1\c$\skyline dataminer\subdir\test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer", @"\\127.0.0.1\c$\skyline dataminer\subdir1\test.txt", @"\\127.0.0.1\c$\skyline dataminer\subdir1\test.txt")]
        public void ConstructSecurePathWithSubdirectoriesSuccess(string basePath, string filename, string expectedResult)
        {
            string result = SecurePath.ConstructSecurePathWithSubDirectories(basePath, filename);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        // straightforward cases
        [DataRow(@"C:\skyline dataminer\test.txt", @"C:\skyline dataminer", @"test.txt")]
        [DataRow(@"C:\skyline dataminer\test.txt", @"C:\skyline dataminer\", @"test.txt")]
        // straightforward cases with subdirectories
        [DataRow(@"C:\skyline dataminer\subdir1\subdir2\test.txt", @"C:\skyline dataminer", @"subdir1", @"subdir2", @"test.txt")]
        [DataRow(@"C:\skyline dataminer\subdir1\subdir2\test.txt", @"C:\skyline dataminer\", @"subdir1", @"subdir2", @"test.txt")]
        // edge case where we traverse out of a subdir
        [DataRow(@"C:\skyline dataminer\subdir1\test.txt", @"C:\skyline dataminer", @"subdir1", @"subdir2", @"..", @"test.txt")]
        [DataRow(@"C:\skyline dataminer\subdir1\test.txt", @"C:\skyline dataminer\", @"subdir1", @"subdir2", @"..", @"test.txt")]
        [DataRow(@"C:\test.txt", @"C:\", @"skyline dataminer", @"subdir1", @"..", @"..", @"test.txt")]
        // network paths
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer\test.txt", @"\\127.0.0.1\c$\skyline dataminer", @"test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer\subdir1\subdir2\test.txt", @"\\127.0.0.1\c$\skyline dataminer", @"subdir1", @"subdir2", @"test.txt")]
        [DataRow(@"\\127.0.0.1\c$\skyline dataminer\subdir1\test.txt", @"\\127.0.0.1\c$\skyline dataminer", @"subdir1", @"subdir2", @"..", @"test.txt")]
        [DataRow(@"\\127.0.0.1\c$\test.txt", @"\\127.0.0.1\c$\", @"skyline dataminer", @"subdir1", @"..", @"..", @"test.txt")]
        public void ConstructSecurePathWithParamsSuccess(string expectedResult, params string[] paths)
        {
            string result = SecurePath.ConstructSecurePath(paths);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        // cases with invalid basedir
        [DataRow(null, @"\subdir\test.txt", typeof(ArgumentException))]
        [DataRow("", @"\subdir\test.txt", typeof(ArgumentException))]
        [DataRow(" ", @"\subdir\test.txt", typeof(ArgumentException))]
        [DataRow("\n", @"\subdir\test.txt", typeof(ArgumentException))]
        // cases with invalid filename
        [DataRow(@"C:\skyline dataminer\", null, typeof(ArgumentException))]
        [DataRow(@"C:\skyline dataminer\", "", typeof(ArgumentException))]
        [DataRow(@"C:\skyline dataminer\", " ", typeof(ArgumentException))]
        [DataRow(@"C:\skyline dataminer\", "\n", typeof(ArgumentException))]
        // cases with invalid characters
        [DataRow(@"C:\skyline dataminer>\", @"test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer\", @"<test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer|\", @"test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer*\", @"test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer?\", @"test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer\", "\"test.txt", typeof(InvalidOperationException))]
        // cases with directory traversal
        [DataRow(@"C:\skyline dataminer", @"..\..\..\test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer", @"C:\test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer", @"\\127.0.0.1\c$\test.txt", typeof(InvalidOperationException))]
        [DataRow(@"%programfiles%", @"test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer", @"%programfiles%\test.txt", typeof(InvalidOperationException))]
        [DataRow(@"%programfiles%", @"C:\skyline dataminer\test.txt", typeof(InvalidOperationException))]
        // cases with subdirectories while it's not allowed
        [DataRow(@"C:\skyline dataminer\", @"subdir\test.txt", typeof(InvalidOperationException))]
        // case where wrong directory delimiter is used
        [DataRow(@"C:/skyline dataminer/", @"test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:/skyline dataminer/", @"test.txt", typeof(InvalidOperationException))]
        [DataRow(@"C:\skyline dataminer\", @"subdir/test.txt", true, typeof(InvalidOperationException))]
        public void ConstructSecurePathFailure(string basePath, string filename, Type expectedExceptionType)
        {
            typeof(Assert)
                .GetMethods()
                .Where(m => m.Name == "ThrowsException")
                .Where(m => m.GetParameters().Length == 1)
                .First(m => m.GetParameters()[0].ParameterType == typeof(Func<object?>))
                .MakeGenericMethod(expectedExceptionType)
                .Invoke(null, new object[] { () => SecurePath.ConstructSecurePath(basePath, filename) });
        }

        [TestMethod]
        //case with invalid arguments
        [DataRow(typeof(ArgumentException), null)]
        [DataRow(typeof(ArgumentException), @"C:\skyline dataminer\")]
        // case with invalid characters in basepath
        [DataRow(typeof(InvalidOperationException), @"C:/skyline dataminer/", @"subdir1", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer>\", @"subdir1", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer<\", @"subdir1", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer|\", @"subdir1", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), "C:\\skyline dataminer\0\\", @"subdir1", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"%programfiles%", @"subdir1", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\..\", @"subdir1", @"test.txt")]
        // case with invalid characters in subdirectories
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer", @"subdir1/", @"subdir2", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1>", @"subdir2", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1<", @"subdir2", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1|", @"subdir2", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", "subdir1\0", @"subdir2", @"test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", "subdir1\"", @"subdir2", @"test.txt")]
        // case with invalid characters in filename
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer", @"subdir1", @"/test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @"<test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @">test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @"|test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", "\0test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", "\"test.txt")]
        // case with directory traversal
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @"..", "test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @"C:\", "test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @"\\127.0.0.1\c$\", "test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @"C:\test.txt")]
        [DataRow(typeof(InvalidOperationException), @"C:\skyline dataminer\", @"subdir1", @"\\127.0.0.1\c$\test.txt")]
        public void ConstructSecurePathWithParamsFailure(Type expectedExceptionType, params string[] paths)
        {
            typeof(Assert)
                .GetMethods()
                .Where(m => m.Name == "ThrowsException")
                .Where(m => m.GetParameters().Length == 1)
                .First(m => m.GetParameters()[0].ParameterType == typeof(Func<object?>))
                .MakeGenericMethod(expectedExceptionType)
                .Invoke(null, new object[] { () => SecurePath.ConstructSecurePath(paths) });
        }

        [TestMethod]
        public void IsPathValidSuccess(string path)
        {
            Assert.IsTrue(SecurePath.IsPathValid(path));
        }

        [TestMethod]
        public void IsPathValidFailure(string path)
        {
            Assert.IsFalse(SecurePath.IsPathValid(path));
        }
    }
}