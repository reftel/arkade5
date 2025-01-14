using System.Collections.Generic;
using Arkivverket.Arkade.Core.Base.Noark5;
using Arkivverket.Arkade.Core.Resources;
using Arkivverket.Arkade.Core.Util;

namespace Arkivverket.Arkade.Core.Testing.Noark5
{
    public class N5_23_NumberOfDocumentDescriptions : Noark5XmlReaderBaseTest
    {
        private readonly TestId _id = new TestId(TestId.TestKind.Noark5, 23);

        private string _currentArchivePartSystemId;
        private int _totalNumberOfDocumentDescriptions;
        private readonly Dictionary<string, int> _documentDescriptionsPerArchivePart = new Dictionary<string, int>();

        public override TestId GetId()
        {
            return _id;
        }

        public override TestType GetTestType()
        {
            return TestType.ContentAnalysis;
        }

        protected override List<TestResult> GetTestResults()
        {
            var testResults = new List<TestResult>
            {
                new TestResult(ResultType.Success, new Location(string.Empty),
                   string.Format(Noark5Messages.TotalResultNumber, _totalNumberOfDocumentDescriptions.ToString()))
            };

            if (_documentDescriptionsPerArchivePart.Count > 1)
            {
                foreach (KeyValuePair<string, int> documentDescriptionCount in _documentDescriptionsPerArchivePart)
                {
                    var testResult = new TestResult(ResultType.Success, new Location(string.Empty),
                        string.Format(Noark5Messages.NumberOf_PerArchivePart,
                            documentDescriptionCount.Key, documentDescriptionCount.Value));

                    testResults.Add(testResult);
                }
            }

            return testResults;
        }

        protected override void ReadStartElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadAttributeEvent(object sender, ReadElementEventArgs eventArgs)
        {
        }

        protected override void ReadElementValueEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.Path.Matches("systemID", "arkivdel"))
            {
                _currentArchivePartSystemId = eventArgs.Value;
                _documentDescriptionsPerArchivePart.Add(_currentArchivePartSystemId, 0);
            }
        }

        protected override void ReadEndElementEvent(object sender, ReadElementEventArgs eventArgs)
        {
            if (eventArgs.NameEquals("dokumentbeskrivelse"))
            {
                _totalNumberOfDocumentDescriptions++;

                if (_documentDescriptionsPerArchivePart.Count >0)
                {
                    if (_documentDescriptionsPerArchivePart.ContainsKey(_currentArchivePartSystemId))
                        _documentDescriptionsPerArchivePart[_currentArchivePartSystemId]++;
                }
              
            }
        }
    }
}