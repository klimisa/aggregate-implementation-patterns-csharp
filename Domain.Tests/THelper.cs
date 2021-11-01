namespace Domain.Tests
{
    using System.Collections.Generic;
    using Shared.Event;

    public class THelper
    {
        public static string TypeOfFirst(List<Event> recordedEvents)
        {
            return recordedEvents.Count == 0 
                ? "???" 
                : recordedEvents[0].GetType().Name;
        }
        
        public static string PropertyIsNull(string property) {
            return $"PROBLEM: The {property} is null!\n" +
                   "HINT: Maybe you didn't apply the previous events properly!?\n";
        }
        
        public static string EventIsNull(string method, string expectedEvent) {
            return $"PROBLEM in {method}(): The recorded/returned event is NULL!\n" +
                   $"HINT: Make sure you record/return a {expectedEvent} event\n\n";
        }
        
        public static string PropertyIsWrong(string method, string property) {
            return $"PROBLEM in {method}(): The event contains a wrong {property}!\n" +
                   $"HINT: The {property} in the event should be taken from the command!\n\n";
        }
        
        public static string NoEventWasRecorded(string method, string expectedEvent) {
            return $"PROBLEM in {method}(): No event was recorded/returned!\n" +
                   $"HINTS: Build a {expectedEvent} event and record/return it!\n" +
                   "       Did you apply all previous events properly?\n" + 
                   "       Check your business logic :-)!\n\n";
        }
        
        public static string EventOfWrongTypeWasRecorded(string method) {
            return $"PROBLEM in {method}(): An event of the wrong type was recorded/returned!\n" +
                   "HINTS: Did you apply all previous events properly?\n" + 
                   "       Check your business logic :-)!\n\n";
        }
        
        public static string NoEventShouldHaveBeenRecorded(string recordedEventType) {
            return "PROBLEM: No event should have been recorded/returned!\n" +
                   "HINTS: Check your business logic - this command should be ignored (idempotency)!\n" +
                   "       Did you apply all previous events properly?\n" +
                   $"       The recorded/returned event is of type {recordedEventType}.\n\n";
        }
    }
}