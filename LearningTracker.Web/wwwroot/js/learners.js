document.addEventListener("DOMContentLoaded", function () {

            const searchInput = document.getElementById("candidateSearch");
            const clearButton = document.getElementById("clearCandidateSearch");
            const candidateCount = document.getElementById("candidateCount");
            const candidateRows = document.querySelectorAll(".candidate-row");
            const noResults = document.getElementById("noSearchResults");

            if (!searchInput) return;

            function filterCandidates() {

                const searchTerm = searchInput.value
                    .trim()
                    .toLowerCase();

                let visibleCount = 0;

                candidateRows.forEach(row => {

                    const searchableText = row.dataset.search.toLowerCase();

                    const matches = searchableText.includes(searchTerm);

                    row.hidden = !matches;

                    if (matches) {
                        visibleCount++;
                    }
                });

                // Update candidate count
                candidateCount.textContent =
                    `${visibleCount} ${visibleCount === 1 ? "Candidate" : "Candidates"}`;

                // Display empty search result message
                noResults.hidden = visibleCount !== 0;

                // Show or hide clear button
                clearButton.hidden = searchTerm.length === 0;
            }

            searchInput.addEventListener("input", filterCandidates);

            clearButton.addEventListener("click", function () {
                searchInput.value = "";
                filterCandidates();
                searchInput.focus();
            });

        });