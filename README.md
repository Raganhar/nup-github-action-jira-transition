only 2 use cases are supported:
- Transition tickets on PR

        uses: Raganhar/nup-github-action-jira-transition@main
        env:
          GITHUB_CONTEXT: "${{ toJson(github) }}"
        with:
          jira-api-key: ${{ secrets.JIRA_API_TOKEN }}
          jira-url: ${{ secrets.JIRA_BASE_URL }}
          jira-user: ${{ secrets.JIRA_USER_EMAIL }}
          main-jira-transition: done
          release-jira-transition: in progress


- Transition tickets on push

        uses: Raganhar/nup-github-action-jira-transition@main
        env:
          GITHUB_CONTEXT: "${{ toJson(github) }}"
        with:
          jira-api-key: ${{ secrets.JIRA_API_TOKEN }}
          jira-url: ${{ secrets.JIRA_BASE_URL }}
          jira-user: ${{ secrets.JIRA_USER_EMAIL }}
          main-jira-transition: done
          release-jira-transition: in progress
          branch_to_compare_to: main

