Feature: Create Read Update Delete Todos
    
Scenario: Create Todo
    Given API is running
    When a TODO with a TITLE is created with an ID
    Then the same TODO can be fetched with the ID